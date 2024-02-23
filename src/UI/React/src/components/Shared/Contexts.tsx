import React, { createContext, useState } from "react";
import { eRequestType } from "../../utils/common/constants";

export interface IFormDirtyParam {
  isDirty: boolean;
  apiCall?:
    | {
        type: eRequestType;
        url: string;
        param: any;
      }
    | undefined;
}

export const IsFormDirtyContext = createContext<{
  param: IFormDirtyParam;
  setIsFormDirty: (param: IFormDirtyParam) => void;
}>({
  param: { isDirty: false },
  setIsFormDirty: () => {},
});

export const GlobalContextProvider: React.FC<
  React.PropsWithChildren<object>
> = ({ children }) => {
  const [param, setIsFormDirty] = useState<IFormDirtyParam>({
    isDirty: false,
  });

  return (
    <IsFormDirtyContext.Provider value={{ param, setIsFormDirty }}>
      {children}
    </IsFormDirtyContext.Provider>
  );
};
