import { createStore } from 'vuex';

const storeService = createStore({
    state() {
        return {
            profileInfo: {
                fullName: '',
                email: '',
            },
        };
    },
    mutations: {
        updateProfileInfo(state, payload) {
            state.profileInfo.fullName = payload.fullName;
            state.profileInfo.email = payload.email;
        },
    },
    getters: {
        getProfileInfo(state) {
            return state.profileInfo;
        },
    },
});

export default storeService;
