import React, {useEffect, useState} from "react";
import {Link, useLocation} from "react-router-dom";
import Title from "antd/lib/typography/Title";
import {Alert, Flex, Spin} from "antd";
import styles from "../../../styles/Markdown.module.css";
import {useSelector} from "react-redux";
import {EditOutlined} from "@ant-design/icons";
import {Problem} from "../../../api/Api.ts";
import {UserState} from "../../../userSlice.ts";
import {judgeApi} from "../../../api/JudgeApi.ts";
import {handleError} from "../../../helpers/handleError.ts";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../../helpers/formatters.ts";
import {SubmitProblem} from "../SubmitProblem.tsx";
import {ProblemSubmits} from "../ProblemSubmits.tsx";

export const TimerProblemDetail: React.FC = () => {
    const location = useLocation();
    const problemId = location.pathname.split("/").pop();
    const [problem, setProblem] = useState<Problem>();
    const [isLoading, setLoading] = useState(true);
    const [lastSubmitId, setLastSubmitId] = useState<number>();
    const api = judgeApi();

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.problemsDetail(Number(213));
            setProblem({
                name: "Time",
                id: Number(problemId),
                languages: response.data.languages,
                memoryLimitBytes: 524288000,
                statement: "",
                timeLimitMilliseconds: 1000,
                isOpened: false,
            });
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [problemId]);

    useEffect(() => {
        if (problem?.name)
            document.title = `${problem?.name} - Judge.NET`;
    }, [problem]);

    const [time, setTime] = useState("00:00:00");

    useEffect(() => {
        const time = () => {
            const event = new Date();
            setTime(event.toUTCString().split(' ')[4]);
        };
        const intervalId = setInterval(time, 1000);

        return () => {
            clearInterval(intervalId);
        };
    }, []);

    return (
        isLoading ? <Spin size="large"/> :
            <>
                <Title style={{textAlign: 'center'}}>{problem?.name} {isAdmin &&
                    <Link to={`/problems/${problemId}/edit`}><EditOutlined/></Link>}</Title>
                <Flex gap="small" vertical>
                    <div style={{textAlign: 'center'}}>
                        Time limit, seconds: {convertMsToSeconds(problem?.timeLimitMilliseconds)}
                    </div>
                    <div style={{textAlign: 'center'}}>
                        Memory limit, megabytes: {convertBytesToMegabytes(problem?.memoryLimitBytes)}
                    </div>
                    <div className={styles.markdown}>
                        <p>Настало время сверить часы.</p>
                        <h3>Input</h3>
                        <p>В этой задаче нет входных данных.</p>
                        <h3>Output</h3>
                        <p>Выведите время на сервере в формате <code>hh:mm:ss</code>.</p>
                        <h3>Example</h3>
                        <table>
                            <thead>
                            <tr>
                                <th>input</th>
                                <th>output</th>
                            </tr>
                            </thead>
                            <tbody>
                            <tr>
                                <td></td>
                                <td>{time}</td>
                            </tr>
                            </tbody>
                        </table>

                        <h3>Note</h3>
                        <p>Ваш ответ должен совпадать с серверным временем на момент проверки.</p>
                    </div>

                    {!user && <Alert type="warning" description={<span>You must <Link
                        to='/login'>login</Link> to submit solutions.</span>}/>}

                    {user && problem && <SubmitProblem languages={problem.languages} problemId={problem.id}
                                                       onSubmit={(submitId) => setLastSubmitId(submitId)}/>}
                    {user && problem &&
                        <ProblemSubmits pageSize={5} problemId={problem.id} userId={user.id}
                                        lastSubmitId={lastSubmitId}/>}
                </Flex>
            </>
    );
};