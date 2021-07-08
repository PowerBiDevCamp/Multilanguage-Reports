
foreach(Table table in Model.Tables) {     
    if(!table.IsHidden){
       // generate translation for table name
       table.TranslatedNames[Model.Culture] = table.Name;
     
       foreach (Column column in table.Columns) {
           // generate translation for column name
           column.TranslatedNames[Model.Culture] = column.Name;
       };

       foreach (Measure measure in table.Measures) {
           // generate translation for measure name
           measure.TranslatedNames[Model.Culture] = measure.Name;
       }; 

       foreach (Hierarchy hierarchy in table.Hierarchies) {
           // generate translation for hierarchy name
           hierarchy.TranslatedNames[Model.Culture] = hierarchy.Name;
       };
    }
}


