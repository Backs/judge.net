import React, {useEffect, useState} from "react";
import {handleError} from "../../helpers/handleError.ts";
import {judgeApi} from "../../api/JudgeApi.ts";
import {Pagination, Table, Tag} from "antd";
import {SubmitStatus} from "../../api/Api.ts";
import {convertBytesToMegabytes, convertMsToSeconds} from "../../helpers/formatters.ts";

export interface ProblemSubmitsProps {
    problemId?: number,
    contestId?: number,
    problemLabel?: string
    userId?: number,
    lastSubmitId?: number
}

interface SubmitInfo {
    key: number,
    submitDate: string;
    language: string;
    status: any;
    passedTests?: number | null;
    totalMilliseconds?: string;
    totalBytes?: string;
}

export const ProblemSubmits: React.FC<ProblemSubmitsProps> = (props) => {
    const api = judgeApi();
    const [submits, setSubmits] = useState<SubmitInfo[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [isLoading, setLoading] = useState<boolean>(true);

    useEffect(() => {

        const data: {
            ProblemId?: number,
            ContestId?: number,
            ProblemLabel?: string,
            UserId?: number,
            Skip: number,
            Take: number
        } = {
            Skip: (pageNumber - 1) * 5,
            Take: 5
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
            const response = await api.api.submitsSubmitsList(data);

            function getColor(status: SubmitStatus) {
                switch (status) {
                    case SubmitStatus.Accepted:
                        return "success";
                    case SubmitStatus.Pending:
                        return "processing";
                }
                return "error";
            }

            const results: SubmitInfo[] = response.data.items.map(p => ({
                key: p.submitResultId,
                language: p.language,
                passedTests: p.passedTests,
                status: <Tag bordered={false} color={getColor(p.status)}>{p.status}</Tag>,
                submitDate: p.submitDate,
                totalBytes: convertBytesToMegabytes(p.totalBytes),
                totalMilliseconds: convertMsToSeconds(p.totalMilliseconds),
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

    }, [pageNumber, props.lastSubmitId]);

    const columns = [
        {
            title: 'Date',
            dataIndex: 'submitDate',
            key: 'submitResultId',
        },
        {
            title: 'Language',
            dataIndex: 'language',
            key: 'submitResultId'
        },
        {
            title: 'Status',
            dataIndex: 'status',
            key: 'submitResultId',
        },
        {
            title: 'Tests passed',
            dataIndex: 'passedTests',
            key: 'submitResultId',
        },
        {
            title: 'Time, s',
            dataIndex: 'totalMilliseconds',
            key: 'submitResultId',
        },
        {
            title: 'Memory, Mb',
            dataIndex: 'totalBytes',
            key: 'submitResultId',
        }
    ];

    return (
        <div>
            <Table dataSource={submits} columns={columns} pagination={false} loading={isLoading}/>
            <Pagination defaultCurrent={1} total={total}
                        defaultPageSize={5} onChange={(page: number) => setPageNumber(page)}/>
        </div>);
};