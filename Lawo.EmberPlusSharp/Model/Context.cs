﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// <copyright>Copyright 2012-2016 Lawo AG (http://www.lawo.com).</copyright>
// Distributed under the Boost Software License, Version 1.0.
// (See accompanying file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Lawo.EmberPlusSharp.Model
{
    internal sealed class Context
    {
        internal Context(IParent parent, int number, string identifier) :
            this(parent, number, identifier, ChildrenRequestPolicy.All)
        {
        }

        internal Context(IParent parent, int number, string identifier, ChildrenRequestPolicy childrenRequestPolicy)
        {
            this.Parent = parent;
            this.Number = number;
            this.Identifier = identifier;
            this.ChildrenRequestPolicy = childrenRequestPolicy;
        }

        internal IParent Parent { get; }

        internal int Number { get; }

        internal string Identifier { get; }

        internal ChildrenRequestPolicy ChildrenRequestPolicy { get; }
    }
}
