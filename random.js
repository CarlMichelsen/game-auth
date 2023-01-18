const rnd = require('crypto').randomBytes(128).toString('base64');
console.log("random string", `"${rnd}"`, "length:", rnd.length);