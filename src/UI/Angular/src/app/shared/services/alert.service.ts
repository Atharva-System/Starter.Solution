import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';
import { AlertNotification } from '../constants/constants';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  showMessage(msg = '', type = AlertNotification.type.success) {
    const toast: any = Swal.mixin({
      toast: true,
      position: 'top',
      showConfirmButton: false,
      timer: 3000,
      customClass: { container: 'toast' },
    });
    toast.fire({
      icon: type,
      title: msg,
      padding: '10px 20px',
    });
  }
}
