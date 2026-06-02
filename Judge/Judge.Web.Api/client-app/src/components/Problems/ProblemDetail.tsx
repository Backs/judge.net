import React, {useEffect, useLayoutEffect, useRef, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {Problem} from "../../api/Api.ts";
import Title from "antd/lib/typography/Title";
import {Alert, Flex, Spin} from "antd";
import Markdown from 'react-markdown'
import remarkGfm from 'remark-gfm'
import rehypeRaw from "rehype-raw";
import styles from "../../styles/Markdown.module.css";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {useSelector} from "react-redux";
import {UserState} from "../../userSlice.ts";
import {SubmitProblem} from "./SubmitProblem.tsx";
import {ProblemSubmits} from "./ProblemSubmits.tsx";
import {EditOutlined} from "@ant-design/icons";

export const ProblemDetail: React.FC = () => {
    const {problemId} = useParams();
    const [problem, setProblem] = useState<Problem>();
    const [isLoading, setLoading] = useState(true);
    const [lastSubmitId, setLastSubmitId] = useState<number>();
    const markdownRef = useRef<HTMLDivElement>(null);
    const api = judgeApi();

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.problemsDetail(Number(problemId));
            setProblem(response.data);
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [problemId]);

    useEffect(() => {
        if (problem?.name)
            document.title = `${problem?.name} - Judge.NET`;
    }, [problem]);

    useLayoutEffect(() => {
        const root = markdownRef.current;
        if (!root) return;

        const scripts = Array.from(root.querySelectorAll('script:not([data-executed])'));

        scripts.forEach((oldScript) => {
            const newScript = document.createElement('script');

            for (const attr of oldScript.attributes) {
                newScript.setAttribute(attr.name, attr.value);
            }

            newScript.textContent = oldScript.textContent || '';
            newScript.setAttribute('data-executed', 'true');

            oldScript.parentNode?.replaceChild(newScript, oldScript);
        });
    }, [problem?.statement]);

    return (
        isLoading ? <Spin size="large"/> :
            <>
                <Title style={{textAlign: 'center'}}>
                    {problem?.name} {isAdmin &&
                    <Link to={`/problems/${problemId}/edit`}><EditOutlined/></Link>}
                </Title>

                <Flex gap="small" vertical>
                    <div style={{textAlign: 'center'}}>
                        Time limit, seconds: {convertMsToSeconds(problem?.timeLimitMilliseconds)}
                    </div>

                    <div style={{textAlign: 'center'}}>
                        Memory limit, megabytes: {convertBytesToMegabytes(problem?.memoryLimitBytes)}
                    </div>

                    <div ref={markdownRef}>
                        <Markdown
                            className={styles.markdown}
                            remarkPlugins={[remarkGfm]}
                            rehypePlugins={[rehypeRaw]}
                            remarkRehypeOptions={{allowDangerousHtml: true}}
                            urlTransform={(value: string) => value}
                        >
                            {problem?.statement}
                        </Markdown>
                    </div>

                    {!user && <Alert
                        type="warning"
                        description={
                            <span>
                                You must <Link to='/login'>login</Link> to submit solutions.
                            </span>
                        }
                    />}

                    {user && problem && (
                        <SubmitProblem
                            languages={problem.languages}
                            problemId={problem.id}
                            onSubmit={(submitId) => setLastSubmitId(submitId)}
                        />
                    )}

                    {user && problem && (
                        <ProblemSubmits
                            pageSize={5}
                            problemId={problem.id}
                            userId={user.id}
                            lastSubmitId={lastSubmitId}
                        />
                    )}
                </Flex>
            </>
    );
};