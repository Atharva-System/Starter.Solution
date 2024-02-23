import Swal from "sweetalert2";
import { NotificationType } from "./common/constants";

const showMessage = (msg = "", type = NotificationType.success) => {
  const toast: any = Swal.mixin({
    toast: true,
    position: "top",
    showConfirmButton: false,
    timer: 3000,
    customClass: { container: "toast" },
  });

  toast.fire({
    icon: type,
    title: msg,
    padding: "10px 20px",
  });
};

export default {
  showMessage,
};
