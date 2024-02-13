import { combineReducers, configureStore } from "@reduxjs/toolkit";
import themeConfigSlice from "./themeConfigSlice";
import userInfoSlice from "./userInfoSlice";

const rootReducer = combineReducers({
  themeConfig: themeConfigSlice,
  userInfo: userInfoSlice,
});

export default configureStore({
  reducer: rootReducer,
});

export type IRootState = ReturnType<typeof rootReducer>;
