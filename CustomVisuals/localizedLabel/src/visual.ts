"use strict";

import "./../style/visual.less";

import powerbi from "powerbi-visuals-api";
import VisualConstructorOptions = powerbi.extensibility.visual.VisualConstructorOptions;
import VisualUpdateOptions = powerbi.extensibility.visual.VisualUpdateOptions;
import IVisual = powerbi.extensibility.visual.IVisual;
import ISandboxExtendedColorPalete = powerbi.extensibility.ISandboxExtendedColorPalette;
import EnumerateVisualObjectInstancesOptions = powerbi.EnumerateVisualObjectInstancesOptions;
import VisualObjectInstance = powerbi.VisualObjectInstance;
import VisualObjectInstanceEnumeration = powerbi.VisualObjectInstanceEnumeration;
import DataView = powerbi.DataView;
import DataViewObjects = powerbi.DataViewObjects;
import Fill = powerbi.Fill;

import * as d3 from "d3";

export interface LabelViewModel {
  IsNotValid: boolean;
  LabelName?: string;
  FontSize?: number;
  FontBold?: boolean;
  FontColor?: string;
  BackgroundColor?: string;
  BorderColor?: string;
  BorderWidth?: number;
  BorderRadius?: number;
  Alignment?: string;
  Padding?: number;
}

export class Visual implements IVisual {

  private svgRoot: d3.Selection<SVGElement, {}, HTMLElement, any>;
  private rect: d3.Selection<SVGElement, {}, HTMLElement, any>;
  private text: d3.Selection<SVGElement, {}, HTMLElement, any>;

  private dataView: DataView;

  private defaultFontSize: number = 14;
  private defaultFontBold: boolean = false;
  private defaultBorderWidth: number = 1;
  private defaultBorderRadius: number = 0;
  private defaultPadding: number = 12;


  private colorPalate: ISandboxExtendedColorPalete;
  private defaultFontColor: string;
  private defaultBackgroundColor: string;
  private defaultBorderColor: string;


  private objectName: string = "localizedLabelProperties";

  constructor(options: VisualConstructorOptions) {
 
    this.colorPalate = options.host.colorPalette;
 
    this.defaultFontColor = this.colorPalate.foregroundDark.value;
    this.defaultBackgroundColor = this.colorPalate.backgroundLight.value;
    this.defaultBorderColor = this.colorPalate.foregroundDark.value;

    this.svgRoot = d3
      .select(options.element)
      .append("svg")
      .attr("width", options.element.clientWidth)
      .attr("height", options.element.clientHeight);

    this.rect = this.svgRoot
      .append("rect")
      .attr("x", 0)
      .attr("y", 0)
      .attr("width", options.element.clientWidth)
      .attr("height", options.element.clientHeight)
      .attr("stroke", "black")
      .attr("stroke-width", "1px")
      .attr("fill", this.colorPalate.backgroundLight.value);

    this.text = this.svgRoot
      .append("text")
      .attr("x", options.element.clientWidth / 2)
      .attr("y", options.element.clientHeight / 2)
      .text("add a localized label field")
      .attr("text-anchor", "middle")
      .attr("dominant-baseline", "central");
  }

  public update(options: VisualUpdateOptions) {

    this.dataView = options.dataViews[0];

    var viewModel = this.getViewModel(options);

    if(viewModel.IsNotValid){
      return;
    }

    var clientWidth: number = options.viewport.width;
    var clientHeight: number = options.viewport.height;

    this.svgRoot
        .attr("width", clientWidth)
        .attr("height", clientHeight);

    this.rect
      .attr("width", clientWidth)
      .attr("height", clientHeight)
      .attr("x", 0)
      .attr("y", 0)
      .attr("fill", viewModel.BackgroundColor)
      .attr("stroke", viewModel.BorderColor)
      .attr("stroke-width", viewModel.BorderWidth)
      .attr("rx", viewModel.BorderRadius)
      .attr("ry", viewModel.BorderRadius);

      let xPosition: number;

      switch(viewModel.Alignment) {
        case "start": {
          xPosition = viewModel.Padding;
          break;
        }
        case "end": {
          xPosition = clientWidth - viewModel.Padding
          break;
        }
        default: {
          xPosition = (clientWidth / 2);
          break;
        }
      }
     
    this.text
      .text(viewModel.LabelName)
      .attr("x", xPosition)
      .attr("y", clientHeight / 2)
      .attr("width", clientWidth)
      .attr("height", clientHeight)
      .attr("font-size", viewModel.FontSize)
      .attr("fill", viewModel.FontColor)
      .attr("font-weight", viewModel.FontBold ? "bold" : "normal")
      .attr("text-anchor", viewModel.Alignment);      

  }

  public getViewModel(options: VisualUpdateOptions): LabelViewModel {
    
    var dataView: DataView = options.dataViews[0];

    if(!dataView){return { IsNotValid: true }; }

    let labelName: string = dataView.metadata.columns[0].displayName;

    let dataViewObjects: DataViewObjects = dataView.metadata.objects;
    let fontSize: number = getValue<number>(dataViewObjects,this.objectName,"fontSize", this.defaultFontSize);
    let fontBold: boolean = getValue<boolean>(dataViewObjects,this.objectName, "fontBold", this.defaultFontBold);
    let fontColor: string = (getValue<Fill>( dataViewObjects,this.objectName,"fontColor",{ solid: { color: this.defaultFontColor  } })).solid.color;
    let backgroundColor: string = (getValue<Fill>( dataViewObjects,this.objectName,"backgroundColor",{ solid: { color: this.defaultBackgroundColor } })).solid.color;
    let borderColor: string =  (getValue<Fill>(dataViewObjects, this.objectName, "borderColor", { solid: { color: this.defaultBorderColor } })).solid.color;
    let borderWidth: number = getValue<number>(dataViewObjects, this.objectName, "borderWidth", this.defaultBorderWidth);
    let borderRadius: number = getValue<number>(dataViewObjects, this.objectName, "borderRadius", this.defaultBorderRadius);
    let alignment: string = getValue<string>(dataViewObjects, this.objectName, "alignment","middle");
    let padding: number = getValue<number>(dataViewObjects, this.objectName, "padding", this.defaultPadding);

    var viewModel: LabelViewModel = { 
      IsNotValid: false,
      LabelName: labelName,
      FontSize:  fontSize,
      FontBold: fontBold,
      FontColor: fontColor,
      BackgroundColor: backgroundColor,
      BorderColor: borderColor,
      BorderWidth: borderWidth,
      BorderRadius: borderRadius,
      Alignment: alignment,
      Padding: padding
    }

    return viewModel;

  }

  public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {
     
    let objectName: string = options.objectName;
    let objectEnumeration: VisualObjectInstance[] = [];
    let dataViewObjects: DataViewObjects = this.dataView.metadata.objects;

    switch (objectName) {
      case this.objectName:
        objectEnumeration.push({
          objectName: objectName,
          displayName: objectName,
          properties: { 
            fontSize: getValue<number>(dataViewObjects, objectName, "fontSize", this.defaultFontSize),
            fontBold: getValue<boolean>(dataViewObjects, objectName, "fontBold", this.defaultFontBold),
            fontColor: getValue<Fill>(dataViewObjects, objectName, "fontColor", { solid: { color:  this.defaultFontColor } }),
            backgroundColor: getValue<Fill>(dataViewObjects, objectName, "backgroundColor", { solid: { color: this.defaultBackgroundColor } }),
            borderColor: getValue<Fill>(dataViewObjects, objectName, "borderColor", { solid: { color: this.defaultBorderColor } }),
            borderWidth: getValue<number>(dataViewObjects, objectName, "borderWidth", this.defaultBorderWidth),
            borderRadius: getValue<number>(dataViewObjects, objectName, "borderRadius", this.defaultBorderRadius),
            alignment: getValue<string>(dataViewObjects, objectName, "alignment","middle"),
            padding: getValue<number>(dataViewObjects, this.objectName, "padding", this.defaultPadding)
          },
          validValues: {
            fontSize: { numberRange: { min: 7, max: 72 } },
            borderWidth: { numberRange: { min: 0, max: 12 } },
            borderRadius: { numberRange: { min: 0, max: 32 } },
            padding: { numberRange: { min: 0, max: 32 } }
          },
          selector: null,
        });
        break;
    }

    return objectEnumeration;
  }
}

export function getValue<T>(
  objects: DataViewObjects,
  objectName: string,
  propertyName: string,
  defaultValue: T
): T {
  if (objects) {
    let object = objects[objectName];
    if (object) {
      let property: T = <T>object[propertyName];
      if (property !== undefined) {
        return property;
      }
    }
  }
  return defaultValue;
}
