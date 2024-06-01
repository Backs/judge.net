import React from "react";
import {useParams} from "react-router-dom";

export const Problem: React.FC = () => {
    const {problemId} = useParams()

    return (
        <div>{problemId}</div>
    );
};