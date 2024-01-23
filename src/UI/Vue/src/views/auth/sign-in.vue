<template>
    <div
        class="flex justify-center items-center min-h-screen bg-[url('/assets/images/map.svg')] dark:bg-[url('/assets/images/map-dark.svg')] bg-cover bg-center">
        <div class="panel sm:w-[480px] m-6 max-w-lg w-full">
            <h2 class="font-bold text-2xl mb-3">Sign In</h2>
            <p class="mb-7">Enter your email and password to login</p>
            <form class="space-y-5" @submit.prevent="submitForm()">
                <div :class="{ 'has-error': v$.email.$error }">
                    <label for="email">Email</label>
                    <input id="email" type="text" placeholder="Enter Email" class="form-input" v-model="email" />
                    <template v-if="isSubmitForm && v$.email.$error">
                        <p class="text-danger mt-1">This can not be empty</p>
                    </template>
                </div>
                <div :class="{ 'has-error': v$.password.$error }">
                    <label for="password">Password</label>
                    <input id="password" type="password" placeholder="Enter Password" class="form-input"
                        v-model="password" />
                    <template v-if="isSubmitForm && v$.password.$error">
                        <p class="text-danger mt-1">This can not be empty</p>
                    </template>
                </div>
                <div>
                    <router-link to="/forgot-password" class="text-primary font-bold hover:underline">Forgot
                        Password</router-link>
                </div>
                <button type="submit" class="btn btn-primary w-full">SIGN IN</button>
            </form>
        </div>
    </div>
</template>
<script lang="ts">
import { useVuelidate } from '@vuelidate/core'
import { required } from '@vuelidate/validators'
import authService from '@/services/auth.service';
import { useMeta } from '@/composables/use-meta';
useMeta({ title: 'Login' });
export default {
    setup() {
        return {
            v$: useVuelidate()
        }
    },
    data() {
        return {
            isSubmitForm: false,
            email: '',
            password: ''
        }
    },
    validations() {
        return {
            email: { required },
            password: { required }
        }
    }, methods: {
        async submitForm() {
            this.isSubmitForm = true

            const isFormCorrect = await this.v$.$validate()
            if (!isFormCorrect) return

            const promise = authService.login({ email: this.email, password: this.password });

            promise.then(rs => {
                this.$router.push("/");
            });
        }
    }
}
</script>