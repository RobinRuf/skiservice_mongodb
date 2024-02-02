db = db.getSiblingDB("skiservice");

db.createCollection("Users", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: [
        "UserName",
        "PasswordHash",
        "PasswordSalt",
        "PasswordInputAttempt",
        "IsLocked",
        "Role",
      ],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        UserName: {
          bsonType: "string",
          minLength: 1,
          maxLength: 100,
        },
        PasswordHash: {
          bsonType: "binData",
          minLength: 1,
        },
        PasswordSalt: {
          bsonType: "binData",
          minLength: 1,
        },
        PasswordInputAttempt: {
          bsonType: "int",
          minimum: 0,
          maximum: 3,
        },
        IsLocked: {
          bsonType: "bool",
        },
        Role: {
          bsonType: "string",
          enum: [0, 1],
        },
      },
      additionalProperties: false,
    },
  },
  validationAction: "warn",
});