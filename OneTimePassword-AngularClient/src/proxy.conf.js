const PROXY_CONFIG = [
  {
    context: [
      "/OneTimePassword",
    ],
    target: "https://localhost:7000",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
