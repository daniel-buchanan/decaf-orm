using System;

namespace decaf.db.common.Exceptions;

public class FeatureNotImplementedException(string reason) : Exception(reason);