import React from "react";
import {Routes, Route} from 'react-router-dom';
import {Problems} from "../components/Problems/Problems.tsx";
import {ProblemDetail} from "../components/Problems/ProblemDetail.tsx";
import {NotFound} from "../components/NotFound.tsx";
import {Main} from "../components/Main/Main.tsx";
import {Login} from "../components/Login.tsx";
import {Register} from "../components/Register.tsx";
import {Submits} from "../components/Problems/Submits.tsx";
import {Contests} from "../components/Contests/Contests.tsx";
import {ContestDetails} from "../components/Contests/ContestDetails.tsx";
import {ContestProblemDetail} from "../components/Contests/ContestProblemDetail.tsx";
import {ContestStandings} from "../components/Contests/ContestStandings.tsx";
import {SubmitResultInfo} from "../components/Problems/SubmitResultInfo.tsx";
import {Administration} from "../components/Administration/Administration.tsx";
import {Languages} from "../components/Administration/Languages.tsx";
import {ProblemEdit} from "../components/Administration/ProblemEdit.tsx";
import {AllProblems} from "../components/Administration/AllProblems.tsx";
import {AllContests} from "../components/Administration/AllContests.tsx";
import {ContestEdit} from "../components/Administration/ContestEdit.tsx";
import {AllUsers} from "../components/Administration/AllUsers.tsx";
import {Help} from "../components/Help.tsx";
import {About} from "../components/About.tsx";
import {TimerProblemDetail} from "../components/Problems/CustomProblems/TimerProblemDetail.tsx";

export const Router: React.FC = () => {
    return (
        <Routes>
            <Route path="/" element={<Main/>}/>
            <Route path="/login" element={<Login/>}/>
            <Route path="/help" element={<Help/>}/>
            <Route path="/about" element={<About/>}/>
            <Route path="/register" element={<Register/>}/>
            <Route path="/problems" element={<Problems/>}/>
            <Route path="/problems/213" element={<TimerProblemDetail/>}/>
            <Route path="/problems/:problemId" element={<ProblemDetail/>}/>
            <Route path="/problems/:problemId/edit" element={<ProblemEdit/>}/>
            <Route path="/problems/new" element={<ProblemEdit/>}/>
            <Route path="/submits" element={<Submits/>}/>
            <Route path="/submit-results/:submitResultId" element={<SubmitResultInfo/>}/>
            <Route path="/contests" element={<Contests/>}/>
            <Route path="/contests/:contestId" element={<ContestDetails/>}/>
            <Route path="/contests/:contestId/edit" element={<ContestEdit/>}/>
            <Route path="/contests/new" element={<ContestEdit/>}/>
            <Route path="/contests/:contestId/standings" element={<ContestStandings/>}/>
            <Route path="/contests/:contestId/:label" element={<ContestProblemDetail/>}/>
            <Route path="/administration" element={<Administration/>}/>
            <Route path="/administration/languages" element={<Languages/>}/>
            <Route path="/administration/problems" element={<AllProblems/>}/>
            <Route path="/administration/contests" element={<AllContests/>}/>
            <Route path="/administration/users" element={<AllUsers/>}/>
            <Route path="/notFound" element={<NotFound/>}/>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
}