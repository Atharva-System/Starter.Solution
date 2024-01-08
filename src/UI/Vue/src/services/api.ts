import axios from "axios";

import env from "../environments/index";

const instance = axios.create({
  baseURL: env.ResourceServer.Endpoint,
  headers: {
    "Content-Type": "application/json",
    'request-source' : 'vue'
  },
});
export default instance;
