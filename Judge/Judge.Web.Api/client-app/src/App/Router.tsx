import React from "react";
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import {Problems} from "../components/Problems/Problems.tsx";
import {ProblemDetail} from "../components/Problems/ProblemDetail.tsx";
import NotFound from "../components/NotFound.tsx";
import {Main} from "../components/Main/Main.tsx";
import {Login} from "../components/Login.tsx";

export const Router: React.FC = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Main/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/problems" element={<Problems/>}/>
                <Route path="/problems/:problemId" element={<ProblemDetail/>}/>
                <Route path="/notFound" element={<NotFound/>}/>
                <Route path="*" element={<NotFound/>}/>
            </Routes>
        </BrowserRouter>
    );
}