import { lazy } from "react";
import { appPaths, authPaths } from "../utils/common/route-paths";
const Users = lazy(() => import("../pages/user/pages/list-user"));
const SignIn = lazy(() => import("../pages/auth/pages/sign-in"));
const ForgotPassword = lazy(
  () => import("../pages/auth/pages/forgot-password")
);

const routes = [
  // dashboard
  {
    path: "/",
    element: <Users />,
    layout: "",
  },
  {
    path: "/" + appPaths.users,
    element: <Users />,
    layout: "",
  },
  {
    path: "/" + authPaths.signin,
    element: <SignIn />,
    layout: "blank",
  },
  {
    path: "/" + authPaths.forgotPassword,
    element: <ForgotPassword />,
    layout: "blank",
  },
];

export { routes };
