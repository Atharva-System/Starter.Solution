<template>
    <div
        class="flex justify-center items-center min-h-screen bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')] bg-cover bg-center">
        <div class="panel sm:w-[480px] m-6 max-w-lg w-full">
            <h2 class="font-bold text-2xl mb-3">Create An Account</h2>
            <form class="space-y-5" @submit.prevent="submitForm()">
                <div class="mb-5 flex justify-between gap-4">
                    <div class="flex-1">
                        <label for="firstName">First Name</label>
                        <input id="firstName" type="text" placeholder="First Name" class="form-input disabled-textbox"
                            disabled v-model="firstName" />
                    </div>
                    <div class="flex-1">
                        <label for="lastName">Last Name</label>
                        <input id="lastName" type="text" placeholder="Last Name" class="form-input disabled-textbox"
                            disabled v-model="lastName" />
                    </div>
                </div>
                <div class="mb-5">
                    <label for="email">Email</label>
                    <input id="email" type="text" placeholder="Email" class="form-input disabled-textbox" disabled
                        v-model="email" />
                </div>
                <div :class="{ 'has-error': v$.password.$error }">
                    <label for="password">Password</label>
                    <input id="password" type="password" placeholder="Enter Password" class="form-input"
                        v-model="password" />
                    <span class="text-white-dark text-xs">Password requirements: At least one uppercase
                        letter (A-Z), one lowercase
                        letter (a-z), and one non-alphanumeric character.</span>
                    <template v-if="isSubmitForm && v$.password.$error">
                        <p class="text-danger mt-1" v-for="error of v$.password.$errors" :key="error.$uid">
                            <span v-if="error.$validator == 'required'">This can not be empty</span>
                            <span v-if="error.$validator == 'passPatern'">Password requirements: At least one uppercase
                                letter (A-Z), one lowercase
                                letter (a-z), and one non-alphanumeric character.</span>
                        </p>
                    </template>
                </div>
                <button type="submit" class="btn btn-primary w-full">CREATE AN ACCOUNTAT</button>
                <div
                    class="relative my-7 h-5 text-center before:w-full before:h-[1px] before:absolute before:inset-0 before:m-auto before:bg-[#ebedf2] dark:before:bg-[#253b5c]">
                    <div class="font-bold text-white-dark bg-white dark:bg-[#0e1726] px-2 relative z-[1] inline-block">
                        <span>OR</span>
                    </div>
                </div>
                <p class="text-center">
                    Back to <router-link :to="signinRoute" class="text-primary font-bold hover:underline">Sign
                        In</router-link>
                </p>
            </form>
        </div>
    </div>
</template>
<script lang="ts">
import { acceptInviteUserApi, getInviteDetails } from '../../common/api-paths';
import { useVuelidate } from '@vuelidate/core'
import { required } from '@vuelidate/validators'
import { useMeta } from '@/composables/use-meta';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { signin } from '../../common/route-paths';
import { HttpStatusCode } from 'axios';
import { passwordValidationPattern } from '@/common/constants';

useMeta({ title: 'Forgot Password' });

export default {
    setup() {
        return {
            v$: useVuelidate()
        }
    },
    data() {
        return {
            signinRoute: signin,
            isSubmitForm: false,
            firstName: '',
            lastName: '',
            email: '',
            password: '',
        }
    },
    validations() {
        return {
            password: { required, passPatern: value => passwordValidationPattern.test(value), },
        }
    },
    created() {
        if (this.$route.query.userId) {
            this.setUserDetails();
        }
    },
    methods: {
        async submitForm() {
            this.isSubmitForm = true

            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return
            const response = await api
                .post(acceptInviteUserApi, {
                    userId: this.$route.query.userId,
                    password: this.password
                });
            if (response.data) {
                messageService.showMessage(response.data.data);
                this.$router.push(this.signinRoute);
            }
        },
        async setUserDetails() {
            const response = await api
                .get(getInviteDetails + this.$route.query.userId);
            if (response.data) {
                if (response.data.statusCode === HttpStatusCode.Conflict) {
                    messageService.showMessage('Invitation has already been accepted!', "error");
                    setTimeout(() => {
                        this.$router.push(this.signinRoute);
                    }, 1000);
                } else {
                    const data = response.data.data;
                    this.firstName = data.firstName
                    this.lastName = data.lastName
                    this.email = data.email
                }
            }
        }
    }
}
</script>
