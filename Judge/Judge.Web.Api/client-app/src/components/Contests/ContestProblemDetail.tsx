import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Contest, ContestStatus, Problem} from "../../api/Api.ts";
import Title from "antd/lib/typography/Title";
import {Flex, Spin} from "antd";
import Markdown from 'react-markdown'
import remarkGfm from 'remark-gfm'
import rehypeRaw from "rehype-raw";
import styles from "../../styles/Markdown.module.css";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {useSelector} from "react-redux";
import {UserState} from "../../userSlice.ts";
import {SubmitProblem} from "../Problems/SubmitProblem.tsx";
import {ProblemSubmits} from "../Problems/ProblemSubmits.tsx";

export const ContestProblemDetail: React.FC = () => {
    const {contestId, label} = useParams();
    const [problem, setProblem] = useState<Problem>();
    const [isLoading, setLoading] = useState(true);
    const [lastSubmitId, setLastSubmitId] = useState<number>();
    const [contest, setContest] = useState<Contest>();
    const api = judgeApi();

    const {user}: UserState = useSelector((state: any) => state.user)

    useEffect(() => {
        const fetchData = async () => {
            const contestResponse = await api.api.contestsDetail(Number(contestId));
            setContest(contestResponse.data);
            const problemResponse = await api.api.contestsDetail2(Number(contestId), label!);

            setProblem(problemResponse.data);
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId, label]);

    useEffect(() => {
        if (problem?.name)
            document.title = `${problem?.name} - Judge.NET`;
    }, [problem]);

    return (
        isLoading ? <Spin size="large"/> :
            <>
                <Title style={{textAlign: 'center'}}>{contest?.name}: {problem?.name}</Title>
                <Flex gap="small" vertical>
                    <div style={{textAlign: 'center'}}>
                        Time limit, seconds: {convertMsToSeconds(problem?.timeLimitMilliseconds)}
                    </div>
                    <div style={{textAlign: 'center'}}>
                        Memory limit, megabytes: {convertBytesToMegabytes(problem?.memoryLimitBytes)}
                    </div>
                    <Markdown
                        className={styles.markdown}
                        remarkPlugins={[remarkGfm]}
                        rehypePlugins={[rehypeRaw]}>{problem?.statement}</Markdown>

                    {contest?.status === ContestStatus.Running && user && problem &&
                        <SubmitProblem languages={problem.languages} problemId={problem.id}
                                       onSubmit={(submitId) => setLastSubmitId(submitId)}/>}
                    {contest?.status === ContestStatus.Running && user && problem &&
                        <ProblemSubmits pageSize={5} problemId={problem.id} userId={user.id}
                                        lastSubmitId={lastSubmitId}/>}
                </Flex>
            </>
    );
};