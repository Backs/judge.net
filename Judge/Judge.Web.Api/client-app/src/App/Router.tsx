import React from "react";
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import {Problems} from "../components/Problems/Problems.tsx";
import {ProblemDetail} from "../components/Problems/ProblemDetail.tsx";
import NotFound from "../components/NotFound.tsx";
import {Main} from "../components/Main/Main.tsx";
import {Login} from "../components/Login.tsx";
import {Register} from "../components/Register.tsx";
import {Submits} from "../components/Problems/Submits.tsx";
import {Contests} from "../components/Contests/Contests.tsx";
import {ContestDetails} from "../components/Contests/ContestDetails.tsx";
import {ContestProblemDetail} from "../components/Contests/ContestProblemDetail.tsx";

export const Router: React.FC = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Main/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/register" element={<Register/>}/>
                <Route path="/problems" element={<Problems/>}/>
                <Route path="/problems/:problemId" element={<ProblemDetail/>}/>
                <Route path="/submits" element={<Submits/>}/>
                <Route path="/contests" element={<Contests/>}/>
                <Route path="/contests/:contestId" element={<ContestDetails/>}/>
                <Route path="/contests/:contestId/:label" element={<ContestProblemDetail/>}/>
                <Route path="/notFound" element={<NotFound/>}/>
                <Route path="*" element={<NotFound/>}/>
            </Routes>
        </BrowserRouter>
    );
}