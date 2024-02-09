import { createBrowserRouter } from "react-router-dom";
import BlankLayout from "../components/Layouts/BlankLayout";
import DefaultLayout from "../components/Layouts/DefaultLayout";
import { routes } from "./routes";
import AuthGuard from "./auth-guard";

const finalRoutes = routes.map((route) => {
  return {
    ...route,
    element:
      route.layout === "blank" ? (
        <BlankLayout>{route.element}</BlankLayout>
      ) : (
        <AuthGuard>
          <DefaultLayout>{route.element}</DefaultLayout>
        </AuthGuard>
      ),
  };
});

const router = createBrowserRouter(finalRoutes);

export default router;
