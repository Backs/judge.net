import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import App from "./App/App.tsx";
import {Provider} from "react-redux";
import store from './store'
import 'normalize.css'
import { BrowserRouter } from 'react-router-dom';


ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <BrowserRouter>
            <Provider store={store}>
                <App/>
            </Provider>
        </BrowserRouter>
    </React.StrictMode>
)
