import React, { createContext, useState } from "react";
import { eRequestType } from "../../utils/common/constants";

export interface IFormDirtyParam {
  isDirty: boolean;
  isFormValid: boolean;
  apiCall?:
    | {
        type: eRequestType;
        url: string;
        param: any;
      }
    | undefined;
}

interface IIsFormDirtyContext {
  param: IFormDirtyParam;
  setIsFormDirty: (param: IFormDirtyParam) => void;
  resetFormDirty: () => void;
}

export const IsFormDirtyContext = createContext<IIsFormDirtyContext>({
  param: { isDirty: false, isFormValid: true },
  setIsFormDirty: () => {},
  resetFormDirty: () => {},
});

export const GlobalContextProvider: React.FC<
  React.PropsWithChildren<object>
> = ({ children }) => {
  const [param, setParam] = useState<IFormDirtyParam>({
    isDirty: false,
    isFormValid: true,
  });

  const setIsFormDirty = (param: IFormDirtyParam) => {
    setParam(param);
  };

  const resetFormDirty = () => {
    setParam({ isDirty: false, isFormValid: true });
  };

  return (
    <IsFormDirtyContext.Provider
      value={{ param, setIsFormDirty, resetFormDirty }}
    >
      {children}
    </IsFormDirtyContext.Provider>
  );
};
