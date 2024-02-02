const api = db.getSiblingDB('skiservice');

db.getSiblingDB('admin').createUser({
  user: 'superadmin',
  pwd: 'superadmin',
  roles: ['root'],
});

api.createRole({
  role: 'godrole',
  privileges: [
    {
      resource: { db: 'skiservice', collection: '' },
      actions: ['find', 'insert', 'update', 'remove'],
    },
  ],
  roles: [],
});

api.createUser({
  user: 'basicuser',
  pwd: 'basicuser',
  roles: ['godrole'],
});