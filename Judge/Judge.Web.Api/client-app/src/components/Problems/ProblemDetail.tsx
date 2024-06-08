import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Problem} from "../../api/Api.ts";
import Title from "antd/lib/typography/Title";
import {Flex, Spin} from "antd";
import Markdown from 'react-markdown'
import remarkGfm from 'remark-gfm'
import rehypeRaw from "rehype-raw";
import styles from "../../styles/Markdown.module.css";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";

export const ProblemDetail: React.FC = () => {
    const {problemId} = useParams();
    const [problem, setProblem] = useState<Problem>();
    const [isLoading, setLoading] = useState(true);
    const api = judgeApi();
    
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


    return (
        isLoading ? <Spin tip="Loading" size="large"/> :
            <>
                <Title style={{textAlign: 'center'}}>{problem?.name}</Title>
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
                </Flex>
            </>
    );
};