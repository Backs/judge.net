import React, {useEffect, useState} from "react";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Modal, Pagination, Table, Tag} from "antd";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";
import {getColor, getStatusText} from "../../helpers/submitStatusHelper.ts";
import {defaultColumns, extendedColumns, SubmitInfo} from "../../helpers/submitsColumns.ts";
import {SubmitResultInfo, SubmitStatus} from "../../api/Api.ts";
import {UserState} from "../../userSlice.ts";
import {useSelector} from "react-redux";

export interface ProblemSubmitsProps {
    problemId?: number,
    contestId?: number,
    problemLabel?: string
    userId?: number,
    lastSubmitId?: number,
    pageSize: number,
    extended?: boolean,
}

export const ProblemSubmits: React.FC<ProblemSubmitsProps> = (props) => {
    const api = judgeApi();
    const [submits, setSubmits] = useState<SubmitInfo[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [isLoading, setLoading] = useState<boolean>(true);
    const [pageSize, setPageSize] = useState<number>(props.pageSize);

    const {user}: UserState = useSelector((state: any) => state.user)
    const isAdmin = user?.roles.includes("admin") || false;

    const getProblemLink = (submit: SubmitResultInfo): any => {
        if (submit.contestInfo) {
            return <a
                href={`contests/${submit.contestInfo.contestId}/${submit.contestInfo.label}`}>{submit.problemName}</a>
        }
        return <a href={`problems/${submit.problemId}`}>{submit.problemName}</a>
    }

    const getSubmitResultLink = (submit: SubmitResultInfo): any => {
        if (isAdmin) {
            return <a href={`/submit-results/${submit.submitResultId}`}>{submit.submitDate}</a>
        }
        return submit.submitDate;
    }

    const getStatus = (p: SubmitResultInfo): any => {
        if (p.status == SubmitStatus.CompilationError && p.compileOutput) {
            return <span>
                <Tag bordered={false} color={getColor(p.status)}>{getStatusText(p.status)}</Tag>
                <Tag color="blue" style={{cursor: 'pointer'}} onClick={_ => {
                    Modal.info({
                        title: 'Compilation errors',
                        content: p.compileOutput,
                        closable: true,
                        width: "800px",
                        style: {whiteSpace: "pre-wrap"}
                    });
                }}>Show error</Tag>
            </span>;
        }
        return <Tag bordered={false} color={getColor(p.status)}>{getStatusText(p.status)}</Tag>;
    }

    useEffect(() => {

        const data: {
            ProblemId?: number,
            ContestId?: number,
            ProblemLabel?: string,
            UserId?: number,
            Skip: number,
            Take: number
        } = {
            Skip: (pageNumber - 1) * pageSize,
            Take: pageSize
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
        if (props.userId) {
            data.UserId = props.userId;
        }

        const fetchData = async () => {
            setLoading(true);
            const response = await api.api.submitsList(data);

            const results: SubmitInfo[] = response.data.items.map(p => ({
                key: p.submitResultId,
                language: p.language,
                passedTests: p.passedTests,
                status: getStatus(p),
                submitDate: getSubmitResultLink(p),
                totalBytes: convertBytesToMegabytes(p.totalBytes),
                totalMilliseconds: convertMsToSeconds(p.totalMilliseconds),
                userName: p.userName,
                problem: getProblemLink(p)
            }));
            setSubmits(results);
            setTotal(response.data.totalCount);
            setLoading(false);
        }

        fetchData().catch(e => handleError(e));

        const interval = setInterval(() => fetchData().catch(e => handleError(e)), 5000)
        return () => {
            clearInterval(interval);
        }

    }, [pageNumber, props.lastSubmitId, pageSize, user]);

    const columns = props.extended ? extendedColumns : defaultColumns;

    return (
        <div>
            <Table dataSource={submits} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={1} total={total}
                        defaultPageSize={pageSize}
                        onChange={(page: number, size: number) => {
                            setPageNumber(page);
                            setPageSize(size);
                        }}/>
        </div>);
};