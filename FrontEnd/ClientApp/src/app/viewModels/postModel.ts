interface IPost {
    id: number,
    title: string,
    message: string,
    dateCreated: string,
    userUserName: string,
    topicId: number,
    comments: IComment[]
}
