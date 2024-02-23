import React, { Suspense } from "react";
import ReactDOM from "react-dom/client";

// Perfect Scrollbar
import "react-perfect-scrollbar/dist/css/styles.css";

// Tailwind css
import "./tailwind.css";

// Router
import { RouterProvider } from "react-router-dom";
import router from "./router/index";

// Redux
import { Provider } from "react-redux";
import store from "./store/index";

import { GlobalContextProvider } from "./components/Shared/Contexts";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Suspense>
      <Provider store={store}>
        <GlobalContextProvider>
          <RouterProvider router={router} />
        </GlobalContextProvider>
      </Provider>
    </Suspense>
  </React.StrictMode>
);
