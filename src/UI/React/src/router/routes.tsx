import { lazy } from "react";
const Users = lazy(() => import("../pages/user/list-user"));
const SignIn = lazy(() => import("../pages/auth/sign-in"));
const ForgotPassword = lazy(() => import("../pages/auth/forgot-password"));

const routes = [
  // dashboard
  {
    path: "/",
    element: <Users />,
    layout: "",
  },
  {
    path: "/users",
    element: <Users />,
    layout: "",
  },
  {
    path: "/sign-in",
    element: <SignIn />,
    layout: "blank",
  },
  {
    path: "/forgot-password",
    element: <ForgotPassword />,
    layout: "blank",
  },
];

export { routes };
