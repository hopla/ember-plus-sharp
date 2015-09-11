﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// <copyright>Copyright 2012-2015 Lawo AG (http://www.lawo.com). All rights reserved.</copyright>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Lawo
{
    using System;

    /// <summary>Provides constants for common exception messages.</summary>
    /// <threadsafety static="true" instance="false"/>
    public static class ExceptionMessages
    {
        /// <summary>The message to pass to an <see cref="ArgumentOutOfRangeException"/> when a number is unexpectedly
        /// &lt;= 0.</summary>
        public const string PositiveNumberRequired = "Positive number required.";

        /// <summary>The message to pass to an <see cref="ArgumentOutOfRangeException"/> when a number is unexpectedly
        /// &lt; 0.</summary>
        public const string NonnegativeNumberRequired = "Nonnegative number required.";
    }
}
