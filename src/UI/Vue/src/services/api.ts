import axios from 'axios';

import env from '../environments/index';
import messageService from './message.service';
import tokenService from './token.service';

const instance = axios.create({
    baseURL: env.ResourceServer.Endpoint,
    headers: {
        'Content-Type': 'application/json',
        'request-source': 'vue',
    },
});

instance.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response) {
            console.error('Response error:', error.response.status, error.response.data);
            if (error.response.status == 0) {
                tokenService.removeUser();
            } else if (error.response.data && error.response.data.Messages.length > 0) {
                messageService.showMessage(error.response.data.Messages[0], 'error');
            } else if (error.response.data.Message) {
                messageService.showMessage(error.response.data.Message, 'error');
            } else {
                messageService.showMessage('Something went wrong, please try again!', 'error');
            }
        } else if (error.request) {
            console.error('Request error:', error.request);
        } else {
            console.error('Error:', error.message);
        }
        return Promise.reject(error);
    },
);

export default instance;
