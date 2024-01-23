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
        handleError(error);
        return Promise.reject(error);
    },
);

function handleError(error: any) {
    if (error.response) {
        handleResponseError(error.response);
    } else if (error.request) {
        console.error('Request error:', error.request);
    } else {
        console.error('Error:', error.message);
    }
}

function handleResponseError(response: any) {
    console.error('Response error:', response.status, response.data);
    if (response.status === 0) {
        tokenService.signOut();
    } else {
        const errorMessage = response.data?.Messages?.[0] || response.data?.Message || 'Something went wrong, please try again!';
        messageService.showMessage(errorMessage, 'error');
    }
}

export default instance;
