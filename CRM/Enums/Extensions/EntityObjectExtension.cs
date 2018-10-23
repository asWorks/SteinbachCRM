using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DAL.Tools;


///http://www.urmanet.ch/?tag=c-30

///

/// Extension method class for the EntityObject class

///

namespace SteinbachCRM
{

    public static class EntityObjectExtension
    {

        //Enable tracking

        private static readonly List<SelfReferencesTracking> _tracking = new List<SelfReferencesTracking>();



        ///

        /// These method makes a 1:1 copy of the original entity object

        ///

        /// The original entity object /// The copied entity object

        public static EntityObject Clone(this EntityObject entityObject)
    {

        //Get constructor for new object

        var newEntityObject = entityObject.GetType().GetConstructor(

        new Type[0]).Invoke(new object[0]);



        _tracking.Add(new SelfReferencesTracking

        {

            EntitySetName = entityObject.EntityKey.EntitySetName,

            OriginalKeys = entityObject.EntityKey,

            NewEntityObject = (EntityObject)newEntityObject

        });



        //Copy all properties and its values of the given type

        var properties = entityObject.GetType().GetProperties();

        foreach (var property in properties)
        {

            try
            {

                var propertyValue = property.GetValue(entityObject, null);

                PropertyInfo myProperty = property;

                if (entityObject.EntityKey.EntityKeyValues.Where(x => x.Key == myProperty.Name).Count() == 0)
                {

                    //Ignore all properties of these types

                    if (property.PropertyType != typeof(EntityKey) &&

                    property.PropertyType != typeof(EntityState) &&

                    property.PropertyType != typeof(EntityReference<>))
                    {

                        //Check, if the property is a complex type (collection), in that

                        //case, some special calls are necessary

                        if (property.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), false).Count() == 1)
                        {

                            //Check for self referencing entities

                            if (propertyValue.GetType() == entityObject.GetType())
                            {

                                //Get the self referenced entity object

                                var selfRefrencedEntityObject =

                                (EntityObject)property.GetValue(entityObject, null);



                                //This variable is used to store the new parent entity objects

                                EntityObject newParentEntityObject = null;



                                //This loops might be replaced by LINQ queries... I didn't try that

                                foreach (var tracking in _tracking.Where(x => x.EntitySetName == selfRefrencedEntityObject.EntityKey.EntitySetName))
                                {

                                    //Check, if the key is in the tracking list

                                    foreach (var newKeyValues in selfRefrencedEntityObject.EntityKey.EntityKeyValues)
                                    {

                                        //Iterate trough the keys and values

                                        foreach (var orgKeyValues in tracking.OriginalKeys.EntityKeyValues)
                                        {

                                            //The key is stored in the tracking list, which means, this is

                                            //the foreign key used by the self referencing property

                                            if (newParentEntityObject == null)
                                            {

                                                if (orgKeyValues.Key == newKeyValues.Key &&

                                                orgKeyValues.Value == newKeyValues.Value)
                                                {

                                                    //Store the parent entity object

                                                    newParentEntityObject = tracking.NewEntityObject;

                                                }

                                            }

                                            else
                                            {

                                                break;

                                            }

                                        }

                                    }



                                    //Set the value to the new parent entity object

                                    property.SetValue(newEntityObject, newParentEntityObject, null);

                                }

                            }

                            else
                            {

                                //Entity collections are always generic

                                if (propertyValue.GetType().IsGenericType)
                                {

                                    //Don't include self references collection, e.g. Orders1, Orders2 etc.

                                    //Check for equality of the types (string comparison)

                                    if (!propertyValue.GetType().GetGenericArguments().First().FullName.Equals(entityObject.GetType().FullName))
                                    {

                                        //Get the entities of the given property

                                        var entities = (RelatedEnd)property.GetValue(entityObject, null);



                                        //Load underlying collection, if not yet done...

                                        if (!entities.IsLoaded) entities.Load();



                                        //Create a generic instance of the entities collection object

                                        var t = typeof(EntityCollection<>).MakeGenericType(

                                        new[] { property.PropertyType.GetGenericArguments()[0] });



                                        var newEntityCollection = Activator.CreateInstance(t);



                                        //Iterate trough the entities collection

                                        foreach (var entity in entities)
                                        {

                                            //Add the found entity to the dynamic generic collection

                                            var addToCollection = newEntityCollection.GetType().GetMethod("Add");

                                            addToCollection.Invoke(

                                            newEntityCollection,

                                                //new object[] {(EntityObject) entity});

                                            new object[] { Clone((EntityObject)entity) });

                                        }



                                        //Set the property value

                                        property.SetValue(newEntityObject, newEntityCollection, null);

                                    }

                                }

                            }

                        }

                        else
                        {

                            //Common task, just copy the simple type property into the new

                            //entity object

                            //property.SetValue(newEntityObject, property.GetValue(entityObject, null), null);
                            if (propertyValue.GetType().BaseType.Name == "EntityReference")
                            {
                                //Simply copy the EntityKey to the new entity object’s EntityReference
                                ((EntityReference)property.GetValue(newEntityObject, null)).EntityKey = ((EntityReference)property.GetValue(entityObject, null)).EntityKey;
                            }
                            else
                            {
                                //Common task, just copy the simple type property into the new entity object
                                property.SetValue(
                                newEntityObject,
                                property.GetValue(entityObject, null), null);
                            }

                        }

                    }

                }

            }

            catch (InvalidCastException ie)
            {

                //Hmm, something happend...

                //Debug.WriteLine(ie.Message);
                //Output.WriteMessage(ie.Message);
                //Trace.WriteLine(ie.Message);
                //Tools.LoggingTool.LogExeption(ie, Tools.LoggingTool.LogState.low);
                DAL.Tools.LoggingTool.LogExeption(ie, DAL.Tools.LoggingTool.LogState.low);

                continue;

            }

            catch (Exception ex)
            {

                //Hmm, something happend...

                if (ex.Message.Contains("Value cannot be null."))
                {
                    LoggingTool.LogExeption(ex,LoggingTool.LogState.low);
                }

                //Debug.WriteLine(ex.Message);
                //Trace.WriteLine(ex.Message);

                //Output.WriteMessage(ex.Message);
                //DAL.LoggingTool.LogExeption(ex,Tools.LoggingTool.LogState.low);
                DAL.Tools.LoggingTool.LogExeption(ex, DAL.Tools.LoggingTool.LogState.low);

                continue;

            }

        }



        return (EntityObject)newEntityObject;

    }

    }

}
