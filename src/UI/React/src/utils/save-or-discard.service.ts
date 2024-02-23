import axiosInstance from "./api.service";
import { eRequestType } from "./common/constants";
import messageService from "./message.service";

const saveUnsavedChanges = async (apiCall: any): Promise<boolean> => {
  if (apiCall) {
    let response: any;
    
    if (apiCall.type === eRequestType.PUT) {
      response = await axiosInstance.put(apiCall.url, apiCall.param);
    } else {
      response = await axiosInstance.post(apiCall.url, apiCall.param);
    }

    if (response.data) {
      messageService.showMessage(response.data.data);
      return true;
    }
  }
  return false;
};

export default saveUnsavedChanges;
