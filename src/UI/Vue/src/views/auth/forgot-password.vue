<template>
    <div
        class="flex justify-center items-center min-h-screen bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')] bg-cover bg-center">
        <div class="panel sm:w-[480px] m-6 max-w-lg w-full">
            <h2 class="font-bold text-2xl mb-3">Forgot Password</h2>
            <p class="mb-7">Enter your email to recover your ID</p>
            <form class="space-y-5" @submit.prevent="submitForm()">
                <div :class="{ 'has-error': v$.email.$error }">
                    <label for="email">Email</label>
                    <input id="email" type="text" placeholder="Enter Email" class="form-input" v-model="email" />
                    <template v-if="isSubmitForm && v$.email.$error">
                        <p class="text-danger mt-1" v-for="error of v$.$errors" :key="error.$uid">
                            <span v-if="error.$validator == 'required'">This can not be empty</span>
                            <span v-if="error.$validator == 'email'">Invalid Email</span>
                        </p>
                    </template>
                </div>
                <button type="submit" class="btn btn-primary w-full">RECOVER</button>
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
import { forgotPasswordApi } from '../../common/api-paths';
import { useVuelidate } from '@vuelidate/core'
import { required, email } from '@vuelidate/validators'
import { useMeta } from '@/composables/use-meta';
import api from '@/services/api';
import messageService from '@/services/message.service';
import { signin } from '../../common/route-paths';

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
            email: '',
        }
    },
    validations() {
        return {
            email: { required, email },
        }
    }, methods: {
        async submitForm() {
            this.isSubmitForm = true

            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return
            const response = await api
                .post(forgotPasswordApi, {
                    email: this.email
                });
            if (response.data) {
                messageService.showMessage(response.data.data);
            }
        }
    }
}
</script>
