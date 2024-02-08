import devEnv from "./env.dev";
import prodEnv from "./env.prod";

const getApiUrl = () => {
  if (process.env.NODE_ENV === "production") {
    return prodEnv.ResourceServer.apiUrl;
  } else {
    return devEnv.ResourceServer.apiUrl;
  }
};

export default getApiUrl;
