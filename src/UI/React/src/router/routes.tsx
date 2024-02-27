import { lazy } from "react";
import { appPaths, authPaths } from "../utils/common/route-paths";
const Users = lazy(() => import("../pages/user/pages/list-user"));
const Profile = lazy(() => import("../pages/user/pages/profile"));
const Projects = lazy(() => import("../pages/project/pages/list-projects"));
const Tasks = lazy(() => import("../pages/task/pages/list-tasks"));
const SignIn = lazy(() => import("../pages/auth/pages/sign-in"));
const ForgotPassword = lazy(
  () => import("../pages/auth/pages/forgot-password")
);
const ResetPassword = lazy(() => import("../pages/auth/pages/reset-password"));
const AcceptInvite = lazy(() => import("../pages/auth/pages/accept-invite"));
import Error404 from "../components/Error404";

const authRoutes = [
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
  {
    path: "/" + authPaths.resetPassword,
    element: <ResetPassword />,
    layout: "blank",
  },
  {
    path: "/" + authPaths.acceptInvitation,
    element: <AcceptInvite />,
    layout: "blank",
  },
];

const appRoutes = [
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
    path: "/" + appPaths.projects,
    element: <Projects />,
    layout: "",
  },
  {
    path: "/" + appPaths.tasks,
    element: <Tasks />,
    layout: "",
  },
  {
    path: "/" + appPaths.profile,
    element: <Profile />,
    layout: "",
  },
];

const errorRoutes = [
  {
    path: "*",
    element: <Error404 />,
    layout: "blank",
  },
];

const routes = [...errorRoutes, ...appRoutes, ...authRoutes];

export { routes };
