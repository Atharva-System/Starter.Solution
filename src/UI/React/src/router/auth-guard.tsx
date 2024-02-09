import React from "react";
import { Navigate } from "react-router-dom";
import LocalStorageService from "../utils/localstorage.service";
import { authPaths } from "../utils/common/route-paths";

const localStorageService = LocalStorageService.getService();

const AuthGuard: React.FC<React.PropsWithChildren<object>> = ({ children }) => {
  if (!localStorageService.isAuthenticated()) {
    return <Navigate to={"/" + authPaths.signin} />;
  }
  return <>{children}</>;
};

export default AuthGuard;
