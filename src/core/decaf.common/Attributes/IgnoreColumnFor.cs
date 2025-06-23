using System;

namespace decaf.common.Attributes;

public static class IgnoreColumnFor
{
    /// <summary>
    /// Ignore the field this is applied to for UPDATE commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AllAttribute() : IgnoreColumnForAttribute(QueryTypes.Select |
                                                           QueryTypes.Insert |
                                                           QueryTypes.Update |
                                                           QueryTypes.Delete);

    /// <summary>
    /// Ignore the field this is applied to for UPDATE commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UpdateAttribute() : IgnoreColumnForAttribute(QueryTypes.Update);

    /// <summary>
    /// Ingore the field this is applied to for INSERT commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InsertAttribute() : IgnoreColumnForAttribute(QueryTypes.Insert);

    /// <summary>
    /// Ingore the field this is applied to for DELETE commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DeleteAttribute() : IgnoreColumnForAttribute(QueryTypes.Delete);

    /// <summary>
    /// Ignore the field this is applied to for SELECT commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectAttribute() : IgnoreColumnForAttribute(QueryTypes.Select);

    /// <summary>
    /// Ignore the field this is applied to for INSERT and UPDATE commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InsertAndUpdateAttribute() : IgnoreColumnForAttribute(QueryTypes.Insert | QueryTypes.Update);
}