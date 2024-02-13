import { createSlice } from "@reduxjs/toolkit";

interface UserInfoState {
  fullName: string;
  email: string;
}

const initialState: UserInfoState = {
  fullName: "",
  email: "",
};

const userInfoSlice = createSlice({
  name: "userInfo",
  initialState,
  reducers: {
    updateUserInfo(state, action) {
      state.fullName = action.payload.fullName;
      state.email = action.payload.email;
    },
  },
});

export const { updateUserInfo } = userInfoSlice.actions;

export default userInfoSlice.reducer;
