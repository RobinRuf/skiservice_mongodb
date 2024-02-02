db = db.getSiblingDB("skiservice");

db.createCollection("Services", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["ServiceName", "Price"],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        ServiceName: {
          bsonType: "string",
          maxLength: 80,
          enum: [
            "Grosser Service",
            "Kleiner Service",
            "Rennskiservice",
            "Bindung montieren und einstellen",
            "Fell zuschneiden",
            "Heisswachsen",
          ],
        },
        Price: {
          bsonType: "string",
        },
      },
    },
  },
  validationAction: "warn",
});