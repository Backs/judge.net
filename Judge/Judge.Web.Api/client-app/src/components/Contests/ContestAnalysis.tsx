import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Flex, Spin} from "antd";
import styles from "../../styles/Markdown.module.css";
import remarkGfm from "remark-gfm";
import rehypeRaw from "rehype-raw";
import Markdown from "react-markdown";
import Title from "antd/lib/typography/Title";
import {ContestAnalysisInfo} from "../../api/Api.ts";

export const ContestAnalysis: React.FC = () => {
    const {contestId} = useParams();
    const api = judgeApi();
    const [isLoading, setLoading] = useState(true);
    const [contest, setContest] = useState<ContestAnalysisInfo>();


    useEffect(() => {
        const fetchData = async () => {
            const response = await api.api.contestsAnalysisDetail(Number(contestId));

            setContest(response.data);

            setLoading(false);
        }

        fetchData().catch(e => handleError(e));
    }, [contestId]);

    return (isLoading ? <Spin size="large"/> :
        <Flex gap="small" vertical>
            <Title style={{textAlign: 'center'}}>{contest?.name}</Title>
            <Markdown
                className={styles.markdown}
                remarkPlugins={[remarkGfm]}
                rehypePlugins={[rehypeRaw]}>{contest?.analysis}
            </Markdown>
        </Flex>);
}