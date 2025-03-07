import React, {useState} from "react";
import {ProblemLanguage} from "../../api/Api.ts";
import {Alert, Button, Flex, Select} from "antd";
import {judgeApi} from "../../api/JudgeApi.ts";
import {handleError} from "../../helpers/handleError.ts";
import TextArea from "antd/lib/input/TextArea";

export interface SubmitProblemProps {
    languages: ProblemLanguage[],
    problemId?: number,
    contestId?: number,
    problemLabel?: string,
    onSubmit?: (submitId: number) => void
}

export const SubmitProblem: React.FC<SubmitProblemProps> = (props) => {
    const values = props.languages.map(p => ({
        value: p.id,
        label: p.name
    }));

    const [languageId, setLanguageId] = useState<number>();
    const [isUpdating, setUpdating] = useState<boolean>(false);
    const [solution, setSolution] = useState<string>();
    const [isLengthValid, setLengthValid] = useState<boolean>(true);

    const submitSolution = async () => {
        setUpdating(true);
        const api = judgeApi();
        const data: {
            LanguageId: number,
            ProblemId?: number,
            ContestId?: number,
            ProblemLabel?: string
            Solution: string
        } = {
            LanguageId: languageId!,
            Solution: solution!
        };

        if (props.problemId) {
            data.ProblemId = props.problemId;
        }
        if (props.contestId) {
            data.ContestId = props.contestId;
        }
        if (props.problemLabel) {
            data.ProblemLabel = props.problemLabel;
        }


        try {
            const response = await api.api.submitsUpdate(data);
            if (props.onSubmit) {
                props.onSubmit(response.data.id);
            }
        } catch (e: any) {
            handleError(e);
        } finally {
            setSolution(undefined);
            setUpdating(false);
        }
    }

    return (
        <Flex gap="middle" align="start" justify="center" style={{width: "100%"}}>
            <Flex gap="small" vertical style={{width: 620}}>
                <h3>Send solution</h3>
                <Select options={values} onChange={value => setLanguageId(value)}/>
                <TextArea
                    value={solution}
                    rows={10}
                    maxLength={20001}
                    style={{fontFamily: "monospace"}}
                    onChange={value => {
                        const text = value.target.value;
                        setSolution(text);
                        setLengthValid(text.length <= 20000);
                    }}/>
                <Button
                    type="primary"
                    disabled={!languageId || isUpdating || !solution || !isLengthValid}
                    onClick={submitSolution}
                >
                    Submit
                </Button>
                {!isLengthValid && <Alert message="The solution must be less than 20000 characters." type="error"/>}
            </Flex>
        </Flex>
    );
};