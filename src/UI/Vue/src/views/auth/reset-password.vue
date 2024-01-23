<template>
    <div
        class="flex justify-center items-center min-h-screen bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')] bg-cover bg-center">
        <div class="panel sm:w-[480px] m-6 max-w-lg w-full">
            <h2 class="font-bold text-2xl mb-3">Set New Password</h2>
            <form class="space-y-5" @submit.prevent="submitForm()">
                <div :class="{ 'has-error': v$.password.$error }">
                    <label for="password">New Password</label>
                    <input id="password" type="password" placeholder="Enter New Password" class="form-input"
                        v-model="password" />
                    <template v-if="isSubmitForm && v$.password.$error">
                        <p class="text-danger mt-1">This can not be empty</p>
                    </template>
                </div>
                <div :class="{ 'has-error': v$.cpassword.$error }">
                    <label for="cpassword">Confirm Password</label>
                    <input id="cpassword" type="password" placeholder="Confirm New Password" class="form-input"
                        v-model="cpassword" />
                    <template v-if="isSubmitForm && v$.cpassword.required.$invalid">
                        <p class="text-danger mt-1">This can not be empty</p>
                    </template>
                    <template v-if="isSubmitForm && v$.cpassword.sameAsPassword.$invalid">
                        <p class="text-danger mt-1">Password do not match</p>
                    </template>
                </div>
                <button type="submit" class="btn btn-primary w-full">RESET PASSWORD</button>
                <div
                    class="relative my-7 h-5 text-center before:w-full before:h-[1px] before:absolute before:inset-0 before:m-auto before:bg-[#ebedf2] dark:before:bg-[#253b5c]">
                    <div class="font-bold text-white-dark bg-white dark:bg-[#0e1726] px-2 relative z-[1] inline-block">
                        <span>OR</span>
                    </div>
                </div>
                <p class="text-center">
                    Go to <router-link :to="signinRoute" class="text-primary font-bold hover:underline">Sign
                        In</router-link>
                </p>
            </form>
        </div>
    </div>
</template>
<script lang="ts">
import { resetPasswordApi } from '../../common/api-paths';
import { useVuelidate } from '@vuelidate/core'
import { required } from '@vuelidate/validators'
import { useMeta } from '@/composables/use-meta';
import api from '@/services/api';
import messageService from '@/services/message.service';
import type { IResetPasswordRequest } from '@/types/reset-password';
import { signin } from '../../common/route-paths';

useMeta({ title: 'Reset Password' });

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
            password: '',
            cpassword: '',
        }
    },
    validations() {
        return {
            password: {
                required: required,
            },
            cpassword: {
                required: required,
                sameAsPassword: (value: any, vm: any) => value === vm.password,
            },
        }
    }, methods: {
        async submitForm() {
            this.isSubmitForm = true

            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return
            const response = await api
                .post(resetPasswordApi, {
                    token: this.$route.query.token,
                    email: this.$route.query.email,
                    newPassword: this.password
                } as IResetPasswordRequest);
            if (response.data) {
                messageService.showMessage(response.data.data);
                this.$router.push(this.signinRoute);
            }
        }
    }
}
</script>
