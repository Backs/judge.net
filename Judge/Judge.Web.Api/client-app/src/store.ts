import {configureStore} from '@reduxjs/toolkit'
import userReducer from "./userSlice.ts";

export default configureStore({
    reducer: {
        user: userReducer
    },
})