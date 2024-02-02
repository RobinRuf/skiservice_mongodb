const api = db.getSiblingDB('skiservice');
const admin = db.getSiblingDB('admin');

if (admin.getUser('superadmin')) {
  admin.dropUser('superadmin');
  print("Eliminated superadmin");
}

if (api.getUser('basicuser')) {
  api.dropUser('basicuser');
  print("Eliminated basicuser");
}

if (api.getRole('godrole')) {
  api.dropRole('godrole');
  print("Eliminated godrole");
}

if (api) {
  api.dropDatabase();
  print("Knocked out the skiservice database");
}