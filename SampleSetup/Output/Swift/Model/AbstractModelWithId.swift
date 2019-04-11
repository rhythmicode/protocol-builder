struct AbstractModelWithId: Codable {
    let Id: String

    let InsertUserName: String
    let InsertDate: string?

    let UpdateUserName: String
    let UpdateDate: string?

    let RemoveIs: Bool
    let RemoveUserName: String
    let RemoveDate: string?
}
