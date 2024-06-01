import React from "react";
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import {Main} from "../components/Main/Main.tsx";
import {Problems} from "../components/Problems/Problems.tsx";
import {Problem} from "../components/Problems/Problem.tsx";

export const Router: React.FC = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/problems" element={<Problems/>}/>
                <Route path="/problems/:problemId" element={<Problem/>}/>
                <Route path="*" element={<Main/>}/>
            </Routes>
        </BrowserRouter>
    );
}