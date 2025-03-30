/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AllProblemInfo {
  /**
   * Problem id
   * @format int64
   */
  id: number;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /** Is problem opened for everyone */
  isOpened: boolean;
}

/** Problem list */
export interface AllProblemsList {
  /** Problems info */
  items: AllProblemInfo[];
  /**
   * Total count of problems
   * @format int32
   */
  totalCount: number;
}

/** Contest */
export interface Contest {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest end date in UTC
   * @format date-time
   */
  endDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
  /** Contest rules */
  rules: ContestRules;
  /** Is opened */
  isOpened: boolean;
  tasks: ContestTask[];
}

/** Contest information */
export interface ContestInfo {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest end date in UTC
   * @format date-time
   */
  endDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
  /** Contest rules */
  rules: ContestRules;
  /** Is opened */
  isOpened: boolean;
}

/** Contest problem result */
export interface ContestProblemResult {
  /**
   * Task points
   * @format int32
   */
  points: number;
  /**
   * Total problem attempts
   * @format int32
   */
  attempts: number;
  /** Problem solve time */
  time?: string | null;
  /** Problem solved by user */
  solved: boolean;
}

export interface ContestResult {
  /**
   * Contest id
   * @format int32
   */
  id: number;
  /**
   * Contest name
   * @minLength 1
   */
  name: string;
  /**
   * Contest start date in UTC
   * @format date-time
   */
  startDate: string;
  /**
   * Contest end date in UTC
   * @format date-time
   */
  endDate: string;
  /**
   * Contest duration
   * @example "00:00:00"
   */
  duration: string;
  /** Contest status */
  status: ContestStatus;
  /** Contest rules */
  rules: ContestRules;
  /** Is opened */
  isOpened: boolean;
  tasks: ContestTask[];
  users: ContestUserResult[];
}

/** Contest rules */
export enum ContestRules {
  Acm = "Acm",
  Points = "Points",
  CheckPoint = "CheckPoint",
}

/** Contest status */
export enum ContestStatus {
  Planned = "Planned",
  Running = "Running",
  Completed = "Completed",
}

/** Contest task */
export interface ContestTask {
  /**
   * Unique task label
   * @minLength 1
   */
  label: string;
  /**
   * Task name
   * @minLength 1
   */
  name: string;
  /** Task solved by current user */
  solved: boolean;
}

/** Contest user results */
export interface ContestUserResult {
  /**
   * User position
   * @format int32
   */
  position: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  tasks: Record<string, ContestProblemResult>;
  /**
   * Total solved tasks
   * @format int32
   */
  solvedCount: number;
  /**
   * Total points
   * @format int32
   */
  points: number;
}

/** List of contests */
export interface ContestsInfoList {
  items: ContestInfo[];
  /**
   * Total contests count
   * @format int32
   */
  totalCount: number;
}

/** Create new user */
export interface CreateUser {
  /**
   * User email
   * @format email
   * @minLength 2
   */
  email: string;
  /**
   * User login
   * @minLength 2
   */
  login: string;
  /**
   * User password
   * @minLength 6
   */
  password: string;
}

export interface CurrentUser {
  /**
   * User id
   * @format int64
   */
  id: number;
  /**
   * User login
   * @minLength 1
   */
  login: string;
  /**
   * User email
   * @minLength 1
   */
  email: string;
  /** User roles */
  roles: string[];
}

/** Creates or edit contest */
export interface EditContest {
  /**
   * Contest id
   * @format int32
   */
  id?: number | null;
  /**
   * Contest name
   * @minLength 1
   * @maxLength 200
   */
  name: string;
  /**
   * Contest start time
   * @format date-time
   */
  startTime: string;
  /**
   * Contest finish time
   * @format date-time
   */
  finishTime: string;
  /**
   * Contest check point
   * @format date-time
   */
  checkPointTime?: string | null;
  /** Contest rules */
  rules: ContestRules;
  /** Contest problems */
  problems?: EditContestProblem[] | null;
  /** If true, only one language can be used for every task */
  oneLanguagePerTask?: boolean;
  /** If contest is shown for participants */
  isOpened?: boolean;
}

/** Contest problem */
export interface EditContestProblem {
  /**
   * Problem id
   * @format int64
   */
  problemId: number;
  /**
   * Problem label in contest
   * @minLength 1
   */
  label: string;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
}

export interface EditLanguage {
  /**
   * Id
   * @format int32
   */
  id?: number | null;
  /**
   * Language name
   * @minLength 1
   */
  name: string;
  /**
   * Language description
   * @minLength 1
   */
  description: string;
  /** Is language compilable */
  isCompilable: boolean;
  /** Compiler path */
  compilerPath?: string | null;
  /** Compiler options template */
  compilerOptionsTemplate?: string | null;
  /**
   * Executable file template
   * @minLength 1
   */
  outputFileTemplate: string;
  /**
   * Run string template
   * @minLength 1
   */
  runStringTemplate: string;
  /** Is hidden */
  isHidden: boolean;
  /** Default file name */
  defaultFileName?: string | null;
  /** Autodetect file name from source code */
  autoDetectFileName: boolean;
}

/** Creates or edit problem */
export interface EditProblem {
  /**
   * Problem id
   * @format int64
   */
  id?: number | null;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /**
   * Memory limits in bytes
   * @format int32
   */
  memoryLimitBytes: number;
  /**
   * Time limits in milliseconds
   * @format int32
   */
  timeLimitMilliseconds: number;
  /**
   * Problem statement in markdown
   * @minLength 1
   */
  statement: string;
  /**
   * Folder path with tests
   * @minLength 1
   */
  testsFolder: string;
  /** Is problem opened in archive */
  isOpened: boolean;
}

/** Language */
export interface Language {
  /**
   * Id
   * @format int32
   */
  id: number;
  /**
   * Language name
   * @minLength 1
   */
  name: string;
  /**
   * Language description
   * @minLength 1
   */
  description: string;
  /** Is language compilable */
  isCompilable: boolean;
  /** Compiler path */
  compilerPath?: string | null;
  /** Compiler options template */
  compilerOptionsTemplate?: string | null;
  /**
   * Executable file template
   * @minLength 1
   */
  outputFileTemplate: string;
  /**
   * Run string template
   * @minLength 1
   */
  runStringTemplate: string;
  /** Is hidden */
  isHidden: boolean;
  /** Default file name */
  defaultFileName?: string | null;
  /** Autodetect file name from source code */
  autoDetectFileName: boolean;
}

/** Language list */
export interface LanguageList {
  /** Languages */
  items: Language[];
}

/** Login information */
export interface Login {
  /**
   * User email
   * @minLength 1
   */
  email: string;
  /**
   * User password
   * @minLength 1
   */
  password: string;
}

/** Login result */
export interface LoginResult {
  /**
   * Authentication token
   * @minLength 1
   */
  token: string;
}

export interface Problem {
  /**
   * Problem id
   * @format int64
   */
  id: number;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /**
   * Memory limit in bytes
   * @format int64
   */
  memoryLimitBytes: number;
  /**
   * Time limit in milliseconds
   * @format int64
   */
  timeLimitMilliseconds: number;
  /**
   * Problem statement in markdown
   * @minLength 1
   */
  statement: string;
  /** Available languages */
  languages: ProblemLanguage[];
  /** Problem is opened for submits */
  isOpened?: boolean;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

/** Problem info */
export interface ProblemInfo {
  /**
   * Problem id
   * @format int64
   */
  id: number;
  /**
   * Problem name
   * @minLength 1
   */
  name: string;
  /** Is problem solve by current user */
  solved: boolean;
  /** Is problem opened for everyone */
  isOpened: boolean;
}

export interface ProblemLanguage {
  /**
   * Id
   * @format int32
   */
  id?: number;
  /** Language name */
  name?: string | null;
}

/** Problem list */
export interface ProblemsList {
  /** Problems info */
  items: ProblemInfo[];
  /**
   * Total count of problems
   * @format int32
   */
  totalCount: number;
}

/** Contest information */
export interface SubmitResultContestInfo {
  /**
   * Contest id
   * @format int32
   */
  contestId: number;
  /**
   * Label of problem in contest
   * @minLength 1
   */
  label: string;
}

/** Submit result information */
export interface SubmitResultExtendedInfo {
  /**
   * Submit result id
   * @format int64
   */
  submitResultId: number;
  /**
   * Submit date
   * @format date-time
   */
  submitDate: string;
  /**
   * Language name
   * @minLength 1
   */
  language: string;
  status: SubmitStatus;
  /**
   * Total tests passed
   * @format int32
   */
  passedTests?: number | null;
  /**
   * Max execution time in milliseconds
   * @format int64
   */
  totalMilliseconds?: number | null;
  /**
   * Max memory consumed in bytes
   * @format int64
   */
  totalBytes?: number | null;
  /**
   * Problem id
   * @format int64
   */
  problemId?: number | null;
  /**
   * Problem name
   * @minLength 1
   */
  problemName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /** Contest information */
  contestInfo?: SubmitResultContestInfo;
  /** Compilation error */
  compileOutput?: string | null;
  /**
   * Source code of solution
   * @minLength 1
   */
  sourceCode: string;
  /** Compiler output */
  compilerOutput?: string | null;
  /** Text output of run.exe */
  runOutput?: string | null;
  /** User host of submit */
  userHost?: string | null;
}

/** Submit result information */
export interface SubmitResultInfo {
  /**
   * Submit result id
   * @format int64
   */
  submitResultId: number;
  /**
   * Submit date
   * @format date-time
   */
  submitDate: string;
  /**
   * Language name
   * @minLength 1
   */
  language: string;
  status: SubmitStatus;
  /**
   * Total tests passed
   * @format int32
   */
  passedTests?: number | null;
  /**
   * Max execution time in milliseconds
   * @format int64
   */
  totalMilliseconds?: number | null;
  /**
   * Max memory consumed in bytes
   * @format int64
   */
  totalBytes?: number | null;
  /**
   * Problem id
   * @format int64
   */
  problemId?: number | null;
  /**
   * Problem name
   * @minLength 1
   */
  problemName: string;
  /**
   * User id
   * @format int64
   */
  userId: number;
  /**
   * User name
   * @minLength 1
   */
  userName: string;
  /** Contest information */
  contestInfo?: SubmitResultContestInfo;
  /** Compilation error */
  compileOutput?: string | null;
}

/** Submit results list */
export interface SubmitResultsList {
  /** Submit results */
  items: SubmitResultInfo[];
  /**
   * Total count of submit results
   * @format int32
   */
  totalCount: number;
}

/** Submit solution result */
export interface SubmitSolutionResult {
  /**
   * Submit id
   * @format int64
   */
  id: number;
}

export enum SubmitStatus {
  Pending = "Pending",
  CompilationError = "CompilationError",
  RuntimeError = "RuntimeError",
  TimeLimitExceeded = "TimeLimitExceeded",
  MemoryLimitExceeded = "MemoryLimitExceeded",
  WrongAnswer = "WrongAnswer",
  Accepted = "Accepted",
  ServerError = "ServerError",
  TooEarly = "TooEarly",
  Unpolite = "Unpolite",
  TooManyLines = "TooManyLines",
  WrongLanguage = "WrongLanguage",
  PresentationError = "PresentationError",
  PRNotFound = "PRNotFound",
  LoginNotFound = "LoginNotFound",
  NotSolvedYet = "NotSolvedYet",
}

/** User information */
export interface User {
  /**
   * User id
   * @format int64
   */
  id: number;
  /**
   * User login
   * @minLength 1
   */
  login: string;
  /**
   * User email
   * @minLength 1
   */
  email: string;
}

/** Users list */
export interface UsersList {
  /** Users */
  items: User[];
  /**
   * Total users count
   * @format int32
   */
  totalCount: number;
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title Judge.NET
 * @version v1
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Admin
     * @name AdminLanguagesList
     * @request GET:/api/admin/languages
     * @secure
     */
    adminLanguagesList: (params: RequestParams = {}) =>
      this.request<LanguageList, any>({
        path: `/api/admin/languages`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Admin
     * @name AdminLanguagesUpdate
     * @request PUT:/api/admin/languages
     * @secure
     */
    adminLanguagesUpdate: (data: EditLanguage, params: RequestParams = {}) =>
      this.request<LanguageList, any>({
        path: `/api/admin/languages`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Admin
     * @name AdminProblemsList
     * @request GET:/api/admin/problems
     * @secure
     */
    adminProblemsList: (
      query?: {
        /**
         * @format int32
         * @default 0
         */
        skip?: number;
        /**
         * @format int32
         * @default 100
         */
        take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<AllProblemsList, any>({
        path: `/api/admin/problems`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsList
     * @summary Search contests
     * @request GET:/api/contests
     * @secure
     */
    contestsList: (
      query?: {
        /** Search contests with finish date in the future */
        UpcomingOnly?: boolean;
        /**
         * Contests to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Max items in result
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ContestsInfoList, any>({
        path: `/api/contests`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsUpdate
     * @summary Create or update contest information
     * @request PUT:/api/contests
     * @secure
     */
    contestsUpdate: (data: EditContest, params: RequestParams = {}) =>
      this.request<EditContest, any>({
        path: `/api/contests`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsDetail
     * @summary Get contest by id
     * @request GET:/api/contests/{contestId}
     * @secure
     */
    contestsDetail: (contestId: number, params: RequestParams = {}) =>
      this.request<Contest, any>({
        path: `/api/contests/${contestId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsDetail2
     * @summary Get contest problem
     * @request GET:/api/contests/{contestId}/{label}
     * @originalName contestsDetail
     * @duplicate
     * @secure
     */
    contestsDetail2: (contestId: number, label: string, params: RequestParams = {}) =>
      this.request<Problem, any>({
        path: `/api/contests/${contestId}/${label}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsEditableDetail
     * @summary Get editable contest information
     * @request GET:/api/contests/{contestId}/editable
     * @secure
     */
    contestsEditableDetail: (contestId: number, params: RequestParams = {}) =>
      this.request<EditContest, any>({
        path: `/api/contests/${contestId}/editable`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contests
     * @name ContestsResultsDetail
     * @summary Get contest results
     * @request GET:/api/contests/{contestId}/results
     * @secure
     */
    contestsResultsDetail: (contestId: number, params: RequestParams = {}) =>
      this.request<ContestResult, any>({
        path: `/api/contests/${contestId}/results`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Login
     * @name LoginTokenCreate
     * @summary Create authorization token
     * @request POST:/api/login/token
     * @secure
     */
    loginTokenCreate: (data: Login, params: RequestParams = {}) =>
      this.request<LoginResult, any>({
        path: `/api/login/token`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsList
     * @summary Search problems
     * @request GET:/api/problems
     * @secure
     */
    problemsList: (
      query?: {
        /**
         * Problem name
         * @minLength 2
         */
        Name?: string;
        /**
         * Problems to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Problems to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ProblemsList, any>({
        path: `/api/problems`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsUpdate
     * @summary Crate or edit new problem
     * @request PUT:/api/problems
     * @secure
     */
    problemsUpdate: (data: EditProblem, params: RequestParams = {}) =>
      this.request<EditProblem, any>({
        path: `/api/problems`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsDetail
     * @summary Get problem by id
     * @request GET:/api/problems/{id}
     * @secure
     */
    problemsDetail: (id: number, params: RequestParams = {}) =>
      this.request<Problem, any>({
        path: `/api/problems/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Problems
     * @name ProblemsEditableDetail
     * @summary Get editable problem by id
     * @request GET:/api/problems/{id}/editable
     * @secure
     */
    problemsEditableDetail: (id: number, params: RequestParams = {}) =>
      this.request<EditProblem, any>({
        path: `/api/problems/${id}/editable`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsList
     * @summary Search submits
     * @request GET:/api/submits
     * @secure
     */
    submitsList: (
      query?: {
        /**
         * Problem id
         * @format int64
         */
        ProblemId?: number;
        /**
         * Contest id
         * @format int32
         */
        ContestId?: number;
        /** Problem label */
        ProblemLabel?: string;
        /**
         * User id
         * @format int64
         */
        UserId?: number;
        /**
         * Submits to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Submits to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<SubmitResultsList, any>({
        path: `/api/submits`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsUpdate
     * @summary Submit solution
     * @request PUT:/api/submits
     * @secure
     */
    submitsUpdate: (
      data: {
        /**
         * Solution
         * @maxLength 20000
         */
        Solution: string;
        /**
         * Language id
         * @format int32
         */
        LanguageId: number;
        /**
         * problem id
         * @format int64
         */
        ProblemId?: number;
        /**
         * Contest id
         * @format int32
         */
        ContestId?: number;
        /** Problem label in contest */
        ProblemLabel?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<SubmitSolutionResult, ProblemDetails>({
        path: `/api/submits`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.FormData,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name ProblemsSubmitsDetail
     * @summary Get submit results for problem
     * @request GET:/api/problems/{problemId}/submits
     * @secure
     */
    problemsSubmitsDetail: (
      problemId: number,
      query?: {
        /**
         * Submit results to skip
         * @format int32
         * @default 0
         */
        skip?: number;
        /**
         * Submit result to take
         * @format int32
         * @default 20
         */
        take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<SubmitResultsList, any>({
        path: `/api/problems/${problemId}/submits`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsDetail
     * @summary Get submit result by id
     * @request GET:/api/submits/{id}
     * @secure
     */
    submitsDetail: (id: number, params: RequestParams = {}) =>
      this.request<SubmitResultExtendedInfo, any>({
        path: `/api/submits/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Submits
     * @name SubmitsRejudgeCreate
     * @summary Rejudge submit
     * @request POST:/api/submits/{id}/rejudge
     * @secure
     */
    submitsRejudgeCreate: (id: number, params: RequestParams = {}) =>
      this.request<SubmitResultExtendedInfo, any>({
        path: `/api/submits/${id}/rejudge`,
        method: "POST",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersMeList
     * @summary Get current user information
     * @request GET:/api/users/me
     * @secure
     */
    usersMeList: (params: RequestParams = {}) =>
      this.request<CurrentUser, any>({
        path: `/api/users/me`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersList
     * @summary Search users
     * @request GET:/api/users
     * @secure
     */
    usersList: (
      query?: {
        /**
         * User name or email
         * @minLength 2
         */
        Name?: string;
        /**
         * Users to skip
         * @format int32
         * @min 0
         * @max 2147483647
         */
        Skip?: number;
        /**
         * Users to take
         * @format int32
         * @min 1
         * @max 100
         */
        Take?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<UsersList, any>({
        path: `/api/users`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Users
     * @name UsersUpdate
     * @summary Create new user
     * @request PUT:/api/users
     * @secure
     */
    usersUpdate: (data: CreateUser, params: RequestParams = {}) =>
      this.request<User, any>({
        path: `/api/users`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}
