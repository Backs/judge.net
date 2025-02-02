import {ColumnType} from "antd/lib/table";

export interface SubmitInfo {
    key: number,
    submitDate: string;
    language: string;
    status: any;
    passedTests?: number | null;
    totalMilliseconds?: string;
    totalBytes?: string;
    userName?: string,
    problem: any,
}

export const defaultColumns: ColumnType<SubmitInfo>[] = [
    {
        title: 'Date',
        dataIndex: 'submitDate',
        key: 'submitResultId',
        align: 'right'
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
        align: 'right'
    },
    {
        title: 'Time, s',
        dataIndex: 'totalMilliseconds',
        key: 'submitResultId',
        align: 'right'
    },
    {
        title: 'Memory, Mb',
        dataIndex: 'totalBytes',
        key: 'submitResultId',
        align: 'right'
    }
];

export const extendedColumns: ColumnType<SubmitInfo>[] = [
    {
        title: 'Date',
        dataIndex: 'submitDate',
        key: 'submitResultId',
        align: 'right'
    },
    {
        title: 'User name',
        dataIndex: 'userName',
        key: 'submitResultId',
    },
    {
        title: 'Problem',
        dataIndex: 'problem',
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
        align: 'right'
    },
    {
        title: 'Time, s',
        dataIndex: 'totalMilliseconds',
        key: 'submitResultId',
        align: 'right'
    },
    {
        title: 'Memory, Mb',
        dataIndex: 'totalBytes',
        key: 'submitResultId',
        align: 'right'
    }
];