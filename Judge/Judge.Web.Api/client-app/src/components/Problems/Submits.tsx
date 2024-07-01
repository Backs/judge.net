import React from "react";
import {ProblemSubmits} from "./ProblemSubmits.tsx";

export const Submits: React.FC = () => {
    document.title = `Submits - Judge.NET`;

    return (
        <ProblemSubmits pageSize={100} extended={true}/>
    );
}